using SiraUtil.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

namespace ContainerTree
{
    internal class TreeAgent : IInitializable
    {
        private readonly Context _context;
        private readonly SiraLog _siraLog;
        private readonly DiContainer _container;
        private readonly TreeBuilder _treeBuilder;

        public TreeAgent(Context context, SiraLog siraLog, DiContainer container, TreeBuilder treeBuilder)
        {
            _context = context;
            _siraLog = siraLog;
            _container = container;
            _treeBuilder = treeBuilder;
        }

        public void Initialize()
        {
            IEnumerable<Context> contexts = Resources.FindObjectsOfTypeAll<Context>().Where(c => c.Container == _container);
            HashSet<Type> installerTypes = new();
            foreach (var context in contexts)
                foreach (var installer in context.Installers)
                    installerTypes.Add(installer.GetType());

            _siraLog.Debug("=== TreeAgent Initialization ===");
            _siraLog.Debug($" > Contexts: {contexts.Count()}");
            _siraLog.Debug($" > Installers: {installerTypes.Count}");
            _siraLog.Debug($" > Scene: {_context.gameObject.scene.name}");
            _siraLog.Debug($" > Parent Containers: {_container.ParentContainers.Select(c => c.GetHashCode().ToString()).Aggregate((a, b) => $"{a}, {b}")}");
            _siraLog.Debug("Generating branch...");

            _treeBuilder.Add(_container, installerTypes);
            _siraLog.Debug("Complete!");
        }
    }
}
