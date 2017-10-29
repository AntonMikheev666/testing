using System;
using System.Text.RegularExpressions;
using FluentAssertions;
using NUnit.Framework;

namespace HomeExercises
{
	public class NumberValidatorTests
	{
	    public NumberValidator Validator;

	    [SetUp]
	    public void Setup()
	    {
	        Validator = new NumberValidator(3, 1);
	    }

        [TestCase(-1, 2, TestName = "OnNegativePrecision")]
        [TestCase(2, -1, TestName = "OnNegativeScale")]
        [TestCase(1, 2, TestName = "OnPrecisionLessThenScale")]
        public void NumberValidator_Shold_ThrowException(int precision, int scale)
        {
            Assert.Throws<ArgumentException>(() => new NumberValidator(precision, scale));
        }


        [TestCase("", TestName = "EmptyString")]
        [TestCase("sdfhs", TestName = "NaN1")]
        [TestCase("a.bc", TestName = "NaN2")]
        public void NumberValidator_TestIncorrectInput(string input)
        {
            Validator.IsValidNumber(input).Should().BeFalse();
        }

        [TestCase("100.0", ExpectedResult = false, TestName = "TooBigNumberSize")]
        [TestCase("10.0", ExpectedResult = true, TestName = "CorrectNumberSize")]
        [TestCase("+10.0", ExpectedResult = false, TestName = "TooBigNumberSizeWithSign")]
        [TestCase("+0.0", ExpectedResult = true, TestName = "CorrectNumberSizeWithSign")]
        public bool NumberValidator_TestPrecision(string input)
		{
		    return Validator.IsValidNumber(input);
		}


        [TestCase("1.00", ExpectedResult = false, TestName = "TooBigFracionPart")]
        [TestCase("1.0", ExpectedResult = true, TestName = "CorrectFracionPart1")]
        [TestCase("+1.0", ExpectedResult = true, TestName = "CorrectFracionPart2")]
        [TestCase("1", ExpectedResult = true, TestName = "CorrectFracionPart3")]
        public bool NumberValidator_TestScale(string input)
        {
            return Validator.IsValidNumber(input);
        }

        [TestCase("1", ExpectedResult = true, TestName = "PositiveNumberWithoutSign")]
        [TestCase("+1.0", ExpectedResult = true, TestName = "PositiveNumberWithSign")]
        [TestCase("-1.0", ExpectedResult = false, TestName = "NegativeNumber")]
        [TestCase("0", ExpectedResult = true, TestName = "Zero")]
        [TestCase("+0", ExpectedResult = true, TestName = "PositiveZero")]
        [TestCase("-0", ExpectedResult = false, TestName = "NegativeZero")]
        public bool NumberValidator_TestOnlyPositiveNumberValidator(string input)
        {
            Validator = new NumberValidator(3, 2, true);
            return Validator.IsValidNumber(input);
        }

        [TestCase("1", ExpectedResult = true, TestName = "PositiveNumberWithoutSign")]
        [TestCase("+1.0", ExpectedResult = true, TestName = "PositiveNumberWithSign")]
        [TestCase("-1.0", ExpectedResult = true, TestName = "NegativeNumber")]
        [TestCase("0", ExpectedResult = true, TestName = "Zero")]
        [TestCase("+0", ExpectedResult = true, TestName = "PositiveZero")]
        [TestCase("-0", ExpectedResult = true, TestName = "NegativeZero")]
        public bool NumberValidator_TestAnyNumberValidator(string input)
        {
            return Validator.IsValidNumber(input);
        }
    }
}