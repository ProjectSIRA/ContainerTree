using Newtonsoft.Json;
using SiraUtil.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
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
            if (_siContainers.Any(si => si.Address == address))
            {
                _siraLog.Debug("Container already detected! Skipping...");
                return;
            }

            _siraLog.Debug("Building new SiContainer...");
            GameObject contextGO = container.Resolve<Context>().gameObject;
            SiContainer siContainer = new($"{contextGO.name} ({contextGO.scene.name})", address, installers, FormatContracts(container));
            _siraLog.Debug("Checking for parent...");
            DiContainer? parent = container.ParentContainers.FirstOrDefault();
            if (parent is not null)
            {
                _siraLog.Debug("Parent detected, calculating address...");
                string parentAddress = ContainerAddress(parent);
                _siraLog.Debug($"Address Found: {parentAddress}");
                _siraLog.Debug($"Looking for parent SiContainer...");
                SiContainer parentSi = _siContainers.FirstOrDefault(si => si.Address == parentAddress);
                _siraLog.Debug($"Parent Status: {parentSi != null}");
                siContainer.Parent = parentSi;
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
                for (int c = i; c < containers.Count; c++)
                {
                    SerializedContainer a = containers.ElementAt(i);
                    SerializedContainer b = containers.ElementAt(c);

                    if (a == b)
                        continue;

                    SiContainer bContainer = _siContainers[c];
                    if (bContainer.Parent is null)
                        continue;

                    if (a.Address != bContainer.Parent.Address)
                        continue;

                    a.Children.Add(b);
                }
            }

            return containers.First();
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
            
            string json = JsonConvert.SerializeObject(container);
            File.WriteAllText("container_tree.json", json);
        }
    }
}