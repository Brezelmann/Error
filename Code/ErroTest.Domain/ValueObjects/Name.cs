using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using ErrorTest.Domain.Exceptions;

namespace ErrorTest.Domain.ValueObjects
{
    /// <summary>
    /// Describes the Name of Person more precisely.
    /// This Value Object can either be a First, Last or Middle Name.
    /// Allowed are the following value Types:
    ///  - Single Name: Max, Julius, Mustermann.
    ///  - Double Name with minus sign: Lisa-Marie, Luca-Joshua.
    ///  - titles of nobility with 'von': Friedrich von Münchhausen.
    ///  - titles of nobility with 'zu' : Graf zu Falkenstein.
    /// </summary>
    public class Name : ValueObject
    {
        #region Members
        /// <summary>
        /// The First Part of the Name
        /// </summary>
        public string FirstNamePart { get; private set; }

        /// <summary>
        /// The Seperator that is used in the Name (if any)
        /// </summary>
        public string NameSeperator { get; private set; }

        /// <summary>
        /// The Last Part of this Name.
        /// </summary>
        public string LastNamePart { get; private set; }
        #endregion

        #region Constructors
        /// <summary>
        /// For Entity Framework Core
        /// </summary>
        private Name()
        { }
        /// <summary>
        /// Constructor to add a new ValueObject.
        /// Validates the Name.
        /// </summary>
        /// <param name="value">The Name</param>
        public Name(string value)
        {
            try
            {
                if (value.Trim().Contains('-')) // Value is double Name with '-' as seperator.
                {
                    var nameParts = value.Split('-');
                    if (nameParts.Length != 2)
                        throw new ArgumentOutOfRangeException(nameof(value), "-",
                            "You cannot have a Name with more than two Names in it.");

                    FirstNamePart = Regex.IsMatch(nameParts[0], @"^[\p{L}]+$")
                        ? nameParts[0]
                        : throw new InvalidOperationException("A Name can only have letters in it.");
                    NameSeperator = "-";
                    LastNamePart = Regex.IsMatch(nameParts[0], @"^[\p{L}]+$")
                        ? nameParts[1]
                        : throw new InvalidOperationException("A Name can only have letters in it.");
                }
                else if (value.Trim().Contains(' ')) //Value is title of Nobility.
                {
                    var nameParts = value.Split(' ');
                    if (nameParts.Length != 3)
                        throw new ArgumentOutOfRangeException(nameof(value), " ",
                            "You cannot have a Name with more than two Names in it.");

                    FirstNamePart = Regex.IsMatch(nameParts[0], @"^[\p{L}]+$")
                        ? nameParts[0]
                        : throw new InvalidOperationException("A Name can only have letters in it.");
                    NameSeperator = nameParts[1].Equals("von")
                        ? nameParts[1]
                        : nameParts[1].Equals("zu")
                            ? nameParts[1]
                            : throw new InvalidOperationException(
                                "You can only have 'von' and 'zu' as Name Seperators", new ArgumentOutOfRangeException(nameof(value), nameParts[1], "Wrong Seperator"));
                    LastNamePart = Regex.IsMatch(nameParts[0], @"^[\p{L}]+$")
                        ? nameParts[2]
                        : throw new InvalidOperationException("A Name can only have letters in it.");
                }
                else //Value is a normal single Name.
                {
                    if (Regex.IsMatch(value.Trim(), @"^[\p{L}]+$"))
                    {
                        FirstNamePart = value.Trim();
                        NameSeperator = "";
                        LastNamePart = "";
                    }
                    else
                    {
                        throw new ArgumentException("Value doesn't correspond to any known Type of Name");
                    }
                }
            }
            catch (ArgumentOutOfRangeException anx)
            {
                if (((string)anx.ActualValue).Equals("-"))
                    throw new WrongNameFormatException("A Name with a '-' in it can only have two parts. e.g. Max-Meier", anx);
                else if (((string)anx.ActualValue).Equals(" "))
                    throw new WrongNameFormatException("A Title of nobility can only have consist of three parts.", anx);
                else
                    throw new WrongNameFormatException("Couldn't find any good Format for the Name.", anx);
            }
            catch (ArgumentException ax)
            {
                throw new WrongNameFormatException("The Name is a Double Name that has not the correct Format Specification.", ax);
            }

            catch (InvalidOperationException ix)
            {
                if (ix.InnerException != null)
                {
                    throw new WrongNameFormatException("The Name Seperator can only be 'von' or 'zu'", ix);
                }
                else
                {
                    throw new WrongNameFormatException("The Name can only contain german alphabetic signs", ix);
                }
            }
        }
        #endregion

        #region Methods
        /// <summary>
        /// Converts a Name ValueObject to a string with one of three formats based on the NameSeperator.
        /// </summary>
        /// <param name="name">The Name Value Object.</param>
        public static implicit operator string(Name name)
        {
            return name.ToString();
        }

        /// <summary>
        /// Converts a string into a Name Object.
        /// </summary>
        /// <param name="value">The String that needs Conversion.</param>
        public static explicit operator Name(string value)
        {
            return new Name(value);
        }

        /// <summary>
        /// Returns the atomic value of this Value Object.
        /// </summary>
        /// <returns></returns>
        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return FirstNamePart;
            yield return NameSeperator;
            yield return LastNamePart;
        }

        /// <summary>
        /// Custom ToString Implementation to support the different Name Formats.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            if (NameSeperator.Equals("-"))
            {
                return $"{FirstNamePart}{NameSeperator}{LastNamePart}";
            }
            else if (NameSeperator.Equals(""))
            {
                return FirstNamePart;
            }
            else
            {
                return $"{FirstNamePart} {NameSeperator} {LastNamePart}";
            }

        }
        #endregion
    }
}