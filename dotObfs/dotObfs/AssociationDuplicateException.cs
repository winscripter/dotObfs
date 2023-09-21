using System;

namespace dotObfs.Core
{
    internal class AssociationDuplicateException : Exception
    {
        public AssociationDuplicateException(string msg)
            : base (msg)
        {
        }

        public AssociationDuplicateException()
        {
        }
    }
}
