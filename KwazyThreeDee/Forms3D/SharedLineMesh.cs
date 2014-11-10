using System.Collections.Generic;

namespace Forms3D
{
    public abstract class SharedLineMesh
    {
        public SharedLineMesh()
        {
            SharedLines = Generate();
        }

        public IList<SharedLine> SharedLines
        {
            private set;
            get;
        }

        protected abstract IList<SharedLine> Generate();
    }
}
