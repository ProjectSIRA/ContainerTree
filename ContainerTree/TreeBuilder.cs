using SiraUtil.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using Zenject;

namespace ContainerTree
{
    internal class TreeBuilder
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
            SiContainer siContainer = new(container.Resolve<Context>().gameObject.scene.name, address, installers, FormatContracts(container));
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

        private static string ContainerAddress(DiContainer container)
        {
            Context context = container.Resolve<Context>();
            return $"{context.gameObject.scene.name}{TransformPath(context.transform)}";
        }

        private IEnumerable<string> FormatContracts(DiContainer container)
        {
            List<string> formattedContracts = new();
            foreach (var contract in container.AllContracts)
            {
                if (contract.Type.Assembly == _assembly)
                    continue;

                if (contract.Identifier is not null)
                    formattedContracts.Add($"{contract.Type.Name} [{contract.Identifier}]");
                else
                    formattedContracts.Add(contract.Type.Name);
            }
            return formattedContracts;
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
    }
}