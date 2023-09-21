using System;
using System.Collections.Generic;

namespace dotObfs.Core
{
    public class LanguageDefinitionAssociations
    {
        public static Dictionary<string, string> AssociationStore
        {
            get;
            private set;
        }
        = new Dictionary<string, string>();

        public static Dictionary<string, string> ReversedAssociationStore
        {
            get;
            private set;
        }
        = new Dictionary<string, string>();

        public static void SetAssociation(string assoc, string correspondsTo)
        {
            if (!CheckAssociationMatch(assoc))
            {
                AssociationStore.Add(assoc, correspondsTo);
                ReversedAssociationStore.Add(correspondsTo, assoc);
            }
            else
                throw new AssociationDuplicateException($"Association {assoc} was already found.");
        }

        public static bool CheckAssociationMatch(string s) => AssociationStore.ContainsKey(s);

        public string this[string idx]
        {
            get => AssociationStore[idx];
            set => AssociationStore[idx] = value;
        }
    }
}
