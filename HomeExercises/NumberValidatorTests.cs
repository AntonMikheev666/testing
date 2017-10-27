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

	public class NumberValidator
	{
		private readonly Regex numberRegex;
		private readonly bool onlyPositive;
		private readonly int precision;
		private readonly int scale;

		public NumberValidator(int precision, int scale = 0, bool onlyPositive = false)
		{
			this.precision = precision;
			this.scale = scale;
			this.onlyPositive = onlyPositive;
			if (precision <= 0)
				throw new ArgumentException("precision must be a positive number");
			if (scale < 0 || scale >= precision)
				throw new ArgumentException("scale must be a non-negative number less or equal than precision");
			numberRegex = new Regex(@"^([+-]?)(\d+)([.,](\d+))?$", RegexOptions.IgnoreCase);
		}

		public bool IsValidNumber(string value)
		{
			// Проверяем соответствие входного значения формату N(m,k), в соответствии с правилом, 
			// описанным в Формате описи документов, направляемых в налоговый орган в электронном виде по телекоммуникационным каналам связи:
			// Формат числового значения указывается в виде N(m.к), где m – максимальное количество знаков в числе, включая знак (для отрицательного числа), 
			// целую и дробную часть числа без разделяющей десятичной точки, k – максимальное число знаков дробной части числа. 
			// Если число знаков дробной части числа равно 0 (т.е. число целое), то формат числового значения имеет вид N(m).

			if (string.IsNullOrEmpty(value))
				return false;

			var match = numberRegex.Match(value);
			if (!match.Success)
				return false;

			// Знак и целая часть
			var intPart = match.Groups[1].Value.Length + match.Groups[2].Value.Length;
			// Дробная часть
			var fracPart = match.Groups[4].Value.Length;

			if (intPart + fracPart > precision || fracPart > scale)
				return false;

			if (onlyPositive && match.Groups[1].Value == "-")
				return false;
			return true;
		}
	}
}