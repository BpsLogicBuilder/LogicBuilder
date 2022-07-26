using System;

namespace ABIS.LogicBuilder.FlowBuilder.Structures
{
    internal class LinkBoundaries : IEquatable<LinkBoundaries>
    {
        internal LinkBoundaries(int start, int finish)
        {
            this.Start = start;
            this.Finish = finish;
        }

        #region Constants
        #endregion Constants

        #region Properties
        /// <summary>
        /// Start
        /// </summary>
        internal int Start
        {
            get;
        }

        /// <summary>
        /// Finish
        /// </summary>
        internal int Finish
        {
            get;
        }
        #endregion Properties

        #region IEquatable<LinkBoundaries> Members
        public bool Equals(LinkBoundaries? other)
        {
            return Start.Equals(other?.Start) && Finish.Equals(other?.Finish);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as LinkBoundaries);
        }

        public override int GetHashCode()
        {
            return this.Start.GetHashCode();
        }
        #endregion IEquatable<LinkBoundaries> Members
    }
}
