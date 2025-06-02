﻿namespace RentApartments.Domain.ValueObjects.Exceptions
{
    /// <summary>
    /// Provides string constants for error messages.
    /// </summary>
    internal static class ExceptionMessages
    {
        public const string TITLE_NOT_NULL_OR_WHITE_SPACE = "The Title of auction lot mustn't be null, empty or consists only of white-space characters";
        public const string DESCRIPTION_NOT_NULL_OR_WHITE_SPACE = "The Description of auction lot mustn't be null, empty or consists only of white-space characters";
        public const string USERNAME_NOT_NULL_OR_WHITE_SPACE = "The Username of auction lot mustn't be null, empty or consists only of white-space characters";
        public const string VALIDATOR_MUST_BE_SPECIFIED = "Validator must be specified for type";
        public const string MONEY_AMOUNT_NON_POSITIVE = "The amount mustn't be non-positive";
        public const string MONEY_AMOUNT_HAS_NOT_MORE_THEN_TWO_DECIMAL_PLACES = "Money amount has not more then two decimal places";
        public const string ADDRESS_NOT_NULL_OR_WHITE_SPACE = "Address cannot be null, empty or whitespace.";
        public const string ADDRESS_LENGTH_OUT_OF_RANGE = "Address length must be between 5 and 250 characters.";

    }
}
