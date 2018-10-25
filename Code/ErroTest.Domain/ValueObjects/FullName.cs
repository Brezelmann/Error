using System;
using System.Collections.Generic;
using ErrorTest.Domain.Exceptions;

namespace ErrorTest.Domain.ValueObjects
{
    /// <summary>
    /// Nested Value Object to describe the Full Name of a User.
    /// </summary>
    public class FullName : ValueObject
    {
        #region Members
        /// <summary>
        /// The First Name of a User.
        /// </summary>
        public Name FirstName { get; private set; }

        /// <summary>
        /// The Last Name of a User.
        /// </summary>
        public Name LastName { get; private set; }
        #endregion

        #region Constructors
        /// <summary>
        /// For Ef Core.
        /// </summary>
        private FullName()
        { }

        /// <summary>
        /// Creates new Instance of a Fullname.
        /// Can only Convert string if Firstname and Lastname are
        /// seperated by '_'.
        /// </summary>
        /// <param name="value">The Full Name of a Person.</param>
        public FullName(string value)
        {
            try
            {
                if (value.Contains('_'))
                {
                    var seperatedNames = value.Split('_');
                    if (seperatedNames.Length != 2)
                        throw new WrongNameFormatException("A Name cannot have '_' in it.");
                    else
                    {
                        FirstName = new Name(seperatedNames[0]);
                        LastName = new Name(seperatedNames[1]);
                    }
                }
                else
                {
                    throw new WrongNameFormatException("A Name cannot have '_' in it.");
                }
            }
            catch (Exception e)
            {
                throw new WrongNameFormatException("Couldn't create a Full Name with the given String.", e);
            }
            
        }
        #endregion

        #region Methods
        /// <summary>
        /// Returns the Atomic Values of this Fullname.
        /// </summary>
        /// <returns></returns>
        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return FirstName;
            yield return LastName;
        }

        /// <summary>
        /// Returns String Representation of this Fullname.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return $"{FirstName} {LastName}";
        }

        /// <summary>
        /// Converts Full Name back to string.
        /// </summary>
        /// <param name="name"></param>
        public static implicit operator string(FullName name)
        {
            return name.ToString();
        }

        /// <summary>
        /// Converts String to FullName.
        /// Ensure that first and LastName are seperated by '_'
        /// </summary>
        /// <param name="value"></param>
        public static explicit operator FullName(string value)
        {
            return new FullName(value);
        }
        #endregion

    }
}