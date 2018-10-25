using System.Collections.Generic;
using System.Linq;

namespace ErrorTest.Domain.ValueObjects
{
    /// <summary>
    /// Represents a Value Object.
    /// </summary>
    public abstract class ValueObject
    {
        #region Methods
        /// <summary>
        /// Overriden Equal Operator
        /// </summary>
        /// <param name="left">ValueObject to the left of the Operator.</param>
        /// <param name="right">ValueObject to the right of the Operator.</param>
        /// <returns>Flag that indicates the Equality of these two ValueObjects.</returns>
        protected static bool EqualOperator(ValueObject left, ValueObject right)
        {
            if (left is null ^ right is null)
            {
                return false;
            }

            return left?.Equals(right) != false;
        }

        /// <summary>
        /// Overriden Not Equal Operator.
        /// </summary>
        /// <param name="left">ValueObject to the left of the Operator.</param>
        /// <param name="right">ValueObject to the right of the Operator.</param>
        /// <returns>Flag that indicates the Equality of these two ValueObjects.</returns>
        protected static bool NotEqualOperator(ValueObject left, ValueObject right)
        {
            return !(EqualOperator(left, right));
        }

        /// <summary>
        /// Returns the atomic values of this valueObject.
        /// </summary>
        /// <returns>the atomic values in this value object.</returns>
        protected abstract IEnumerable<object> GetAtomicValues();

        /// <summary>
        /// Overridden Equal Method that compares the two objects.
        /// </summary>
        /// <param name="obj">The Object to the right side of the Comparison.</param>
        /// <returns>A FLag indicating the Equality of these Value Objects.</returns>
        public override bool Equals(object obj)
        {
            if (obj == null || obj.GetType() != GetType())
            {
                return false;
            }

            var other = (ValueObject)obj;
            var thisValues = GetAtomicValues().GetEnumerator();
            var otherValues = other.GetAtomicValues().GetEnumerator();

            while (thisValues.MoveNext() && otherValues.MoveNext())
            {
                if (thisValues.Current is null ^ otherValues.Current is null)
                {
                    return false;
                }

                if (thisValues.Current != null &&
                    !thisValues.Current.Equals(otherValues.Current))
                {
                    return false;
                }
            }

            return !thisValues.MoveNext() && !otherValues.MoveNext();
        }

        /// <summary>
        /// Gets the Hashcode of this ValueObject.
        /// </summary>
        /// <returns>A Hashcode.</returns>
        public override int GetHashCode()
        {
            return GetAtomicValues()
                .Select(x => x != null ? x.GetHashCode() : 0)
                .Aggregate((x, y) => x ^ y);
        }
        #endregion

    }
}