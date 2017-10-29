using FluentAssertions;
using HomeExercises.ClassesToTest;
using NUnit.Framework;


namespace HomeExercises.Tests
{
	public class ObjectComparison
	{
		[Test]
		[Description("Проверка текущего царя")]
		[Category("ToRefactor")]
		public void CheckCurrentTsar()
		{
			var actualTsar = TsarRegistry.GetCurrentTsar();

			var expectedTsar = new Person("Ivan IV The Terrible", 54, 170, 70,
				new Person("Vasili III of Russia", 28, 170, 60, null));

			// Перепишите код на использование Fluent Assertions.
		    actualTsar.ShouldBeEquivalentTo(expectedTsar, 
                opt => opt.Excluding(p => (p.SelectedMemberInfo.DeclaringType == typeof(Person))
                                          && (p.SelectedMemberInfo.Name == nameof(Person.Id)))
                          .ComparingEnumsByValue());
		}

		[Test]
		[Description("Альтернативное решение. Какие у него недостатки?")]
		public void CheckCurrentTsar_WithCustomEquality()
		{
			var actualTsar = TsarRegistry.GetCurrentTsar();
			var expectedTsar = new Person("Ivan IV The Terrible", 54, 170, 70,
			new Person("Vasili III of Russia", 28, 170, 60, null));

			// Какие недостатки у такого подхода?
            /*От Антона:
             * Прописывать сравнение каждого поля ручками.
             * Будет лучше, если реализовать функционал AreEqual в методе
             * Equals внутри класса Person.
             */ 
			Assert.True(AreEqual(actualTsar, expectedTsar));

		}

		private bool AreEqual(Person actual, Person expected)
		{
			if (actual == expected) return true;
			if (actual == null || expected == null) return false;
			return
			actual.Name == expected.Name
			&& actual.Age == expected.Age
			&& actual.Height == expected.Height
			&& actual.Weight == expected.Weight
			&& AreEqual(actual.Parent, expected.Parent);
		}
	}
}
