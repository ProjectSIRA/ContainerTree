using System;
using System.Collections.Generic;

namespace ContainerTree
{
    internal class SiContainer
    {
        public string Name { get; }
        public string Address { get; }
        public IEnumerable<Type> Installers { get; }
        public IEnumerable<string> Contracts { get; }
        public SiContainer? Parent { get; set; }

        public SiContainer(string name, string address, IEnumerable<Type> installerTypes, IEnumerable<string> contracts)
        {
            Name = name;
            Address = address;
            Contracts = contracts;
            Installers = installerTypes;
        }
    }
}