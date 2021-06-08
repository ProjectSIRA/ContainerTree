using IPA.Utilities;
using Newtonsoft.Json;
using SiraUtil.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEngine;
using Zenject;

namespace ContainerTree
{
    internal class TreeBuilder : IDisposable
    {
        private readonly SiraLog _siraLog;
        private readonly Assembly _assembly;
        private readonly List<SiContainer> _siContainers = new();

        public TreeBuilder(SiraLog siraLog)
        {
            _siraLog = siraLog;
            _assembly = Assembly.GetExecutingAssembly();
        }

        public void Add(DiContainer container, IEnumerable<Type> installers)
        {
            _siraLog.Debug($"Adding new container... {container.GetHashCode()}");
            string address = ContainerAddress(container);
            _siraLog.Debug($"With Address: {address}");

            if (SelfContainer(container, installers) is not null)
            {
                _siraLog.Debug("Already build container.");
                return;
            }

            _siraLog.Debug("Building new SiContainer...");
            Context context = container.Resolve<Context>();
            GameObject contextGO = context.gameObject;
            SiContainer siContainer = new($"{contextGO.name} ({contextGO.scene.name})", address, installers, FormatContracts(container), container.GetHashCode());
            _siraLog.Debug("Checking for parent...");
            DiContainer? parent = container.ParentContainers.FirstOrDefault();
            if (parent is not null)
            {
                if (context is GameObjectContext)
                    parent = parent.ParentContainers.First();

                _siraLog.Debug("Parent detected, setting value...");
                siContainer.Parent = _siContainers.FirstOrDefault(si => si.CID == parent.GetHashCode());
            }
            else
            {
                _siraLog.Debug("No parent found.");
            }
            _siContainers.Add(siContainer);
        }

        public SerializedContainer? Serialize()
        {
            SiContainer? root = _siContainers.FirstOrDefault();
            if (root is null)
                return null;

            // Generate serialized containers
            HashSet<SerializedContainer> containers = new();
            foreach (var container in _siContainers)
            {
                SerializedContainer serializedContainer = new()
                {
                    Name = container.Name,
                    Address = container.Address,
                    Contracts = container.Contracts.ToArray(),
                    Installers = container.Installers.Select(i => i.Name).ToArray()
                };
                containers.Add(serializedContainer);
            }

            // Populate children (since we know that the oldest containers will be first, we don't have to reorder the list)
            for (int i = 0; i < containers.Count; i++)
            {
                for (int c = 0; c < containers.Count; c++)
                {
                    SerializedContainer a = containers.ElementAt(i);
                    SerializedContainer b = containers.ElementAt(c);

                    if (a == b)
                        continue;

                    SiContainer aContainer = _siContainers[i];
                    SiContainer bContainer = _siContainers[c];

                    if (bContainer.Parent is null)
                        continue;

                    if (aContainer.Address + InstallerString(aContainer.Installers) != bContainer.Parent.Address + InstallerString(bContainer.Parent.Installers))
                        continue;

                    a.Children.Add(b);
                }
            }

            return containers.First();
        }

        private string InstallerString(IEnumerable<Type> installers)
        {
            StringBuilder sb = new();
            foreach (var installer in installers)
                sb.Append(installer.Name);
            return sb.ToString();
        }

        private SiContainer? SelfContainer(DiContainer container, IEnumerable<Type> installers)
        {
            string address = ContainerAddress(container);
            foreach (var potentialParent in _siContainers)
            {
                if (potentialParent.Address != address)
                    continue;

                if (potentialParent.Installers.Count() == installers.Count())
                {
                    bool differentMatch = false;
                    for (int i = 0; i < potentialParent.Installers.Count(); i++)
                    {
                        if (potentialParent.Installers.ElementAt(i) != installers.ElementAt(i))
                        {
                            differentMatch = true;
                            break;
                        }
                    }
                    if (differentMatch)
                        continue;

                    return potentialParent;
                }
            }
            return null;
        }

        private IEnumerable<string> FormatContracts(DiContainer container)
        {
            List<string> formattedContracts = new();
            foreach (var contract in container.AllContracts)
            {
                if (contract.Type.Assembly == _assembly)
                    continue;

                string visualTypeName = RemoveScuff(VisualizedType(contract.Type));
                if (contract.Identifier is not null)
                    formattedContracts.Add($"{visualTypeName} [{contract.Identifier}]");
                else
                    formattedContracts.Add(visualTypeName);
            }
            return formattedContracts;
        }

        private static string VisualizedType(Type type)
        {
            if (type.IsNested)
            {
                return $"{type.DeclaringType.Name}.{type.Name}";
            }
            return type.Name;
        }

        private static string ContainerAddress(DiContainer container)
        {
            Context context = container.Resolve<Context>();
            return $"{context.gameObject.scene.name}{TransformPath(context.transform)}";
        }

        private static string TransformPath(Transform currentTransform)
        {
            if (currentTransform.parent == null)
            {
                return "/" + currentTransform.name;
            }
            else
            {
                return TransformPath(currentTransform.parent) + "/" + currentTransform.name;
            }
        }

        private static string RemoveScuff(string typeName)
        {
            if (typeName.Contains("`"))
            {
                typeName = typeName.Substring(0, typeName.IndexOf("`"));
            }
            return typeName;
        }

        public void Dispose()
        {
            SerializedContainer? container = Serialize();
            if (container is null)
                return;

            ContainerRoot root = new()
            {
                Root = container,
                Version = UnityGame.GameVersion.ToString(),
            };
            string json = JsonConvert.SerializeObject(root);
            File.WriteAllText("container_tree.json", json);
        }
    }
}