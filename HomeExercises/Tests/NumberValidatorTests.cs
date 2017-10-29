using System;
using HomeExercises.ClassesToTest;
using FluentAssertions;
using NUnit.Framework;

namespace HomeExercises.Tests
{
	public class NumberValidatorTests
	{

        [TestCase(-1, 2, TestName = "OnNegativePrecision")]
        [TestCase(2, -1, TestName = "OnNegativeScale")]
        [TestCase(1, 2, TestName = "OnPrecisionLessThenScale")]
        [TestCase(0, 1, TestName = "OnZeroPrecision")]
        public void NumberValidator_Shold_ThrowException(int precision, int scale)
        {
            Assert.Throws<ArgumentException>(() => new NumberValidator(precision, scale));
        }


        [TestCase(3, 1, "", TestName = "EmptyString")]
        [TestCase(3, 1, null, TestName = "Null")]
        [TestCase(3, 1, "sdf", TestName = "Word")]
        [TestCase(3, 1, "a.bc", TestName = "NumberLikeWord")]
        public void NumberValidator_TestIncorrectInput(int precision, int scale, string input)
        {
            new NumberValidator(precision, scale).IsValidNumber(input).Should().BeFalse();
        }

        [TestCase(3, 1, "100.0", ExpectedResult = false, TestName = "TooBigNumberSize")]
        [TestCase(3, 1, "10.0", ExpectedResult = true, TestName = "CorrectNumberSize")]
        [TestCase(3, 1, "+10.0", ExpectedResult = false, TestName = "TooBigNumberSizeWithPlus")]
        [TestCase(3, 1, "+1.0", ExpectedResult = true, TestName = "CorrectNumberSizeWithPlus")]
        [TestCase(3, 1, "-10.0", ExpectedResult = false, TestName = "TooBigNumberSizeWithMinus")]
        [TestCase(3, 1, "-1.0", ExpectedResult = true, TestName = "CorrectNumberSizeWithMinus")]
        public bool NumberValidator_TestPrecision(int precision, int scale, string input)
		{
		    return new NumberValidator(precision, scale).IsValidNumber(input);
		}


        [TestCase(3, 1, "1.00", ExpectedResult = false, TestName = "TooBigFracionPart")]
        [TestCase(3, 1, "1.0", ExpectedResult = true, TestName = "CorrectFracionPart")]
        [TestCase(3, 1, "+1.00", ExpectedResult = false, TestName = "TooBigFracionPartWithPlus")]
        [TestCase(3, 1, "-1.00", ExpectedResult = false, TestName = "TooBigFracionPartWithMinus")]
        [TestCase(3, 1, "+1.0", ExpectedResult = true, TestName = "CorrectFracionPartWithPlus")]
        [TestCase(3, 1, "-1.0", ExpectedResult = true, TestName = "CorrectFracionPartWithMinus")]
        [TestCase(3, 1, "1", ExpectedResult = true, TestName = "NumberWithoutFracionPart")]
        public bool NumberValidator_TestScale(int precision, int scale, string input)
        {
            return new NumberValidator(precision, scale).IsValidNumber(input);
        }

        [TestCase(3, 1, "1.0", ExpectedResult = true, TestName = "PositiveNumberWithoutSign")]
        [TestCase(3, 1, "+1.0", ExpectedResult = true, TestName = "PositiveNumberWithSign")]
        [TestCase(3, 1, "-1.0", ExpectedResult = false, TestName = "NegativeNumber")]
        [TestCase(3, 1, "0.0", ExpectedResult = true, TestName = "Zero")]
        [TestCase(3, 1, "+0.0", ExpectedResult = true, TestName = "PositiveZero")]
        [TestCase(3, 1, "-0.0", ExpectedResult = false, TestName = "NegativeZero")]
        public bool NumberValidator_TestOnlyPositiveNumberValidator(int precision, int scale, string input)
        {
            return new NumberValidator(precision, scale, true).IsValidNumber(input);
        }

        [TestCase(3, 1, "1", ExpectedResult = true, TestName = "PositiveNumberWithoutSignAndFractPart")]
        [TestCase(3, 1, "-1", ExpectedResult = true, TestName = "NumberWithoutFractPart")]
        [TestCase(3, 1, "+1.0", ExpectedResult = true, TestName = "PositiveNumberWithSign")]
        [TestCase(3, 1, "-1.0", ExpectedResult = true, TestName = "NegativeNumber")]
        [TestCase(3, 1, "0.0", ExpectedResult = true, TestName = "ZeroWithFractionPart")]
        [TestCase(3, 1, "+0.0", ExpectedResult = true, TestName = "PositiveZeroWithFractionPart")]
        [TestCase(3, 1, "-0.0", ExpectedResult = true, TestName = "NegativeZeroWithFractionPart")]
        [TestCase(3, 1, "0", ExpectedResult = true, TestName = "Zero")]
        [TestCase(3, 1, "+0", ExpectedResult = true, TestName = "PositiveZero")]
        [TestCase(3, 1, "-0", ExpectedResult = true, TestName = "NegativeZero")]
        public bool NumberValidator_PositiveAndNegativeNumberValidatorTest(int precision, int scale, string input)
        {
            return new NumberValidator(precision, scale).IsValidNumber(input);
        }
    }
}