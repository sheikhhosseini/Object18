using RandomDataGenerator.FieldOptions;
using RandomDataGenerator.Randomizers;

namespace TestProject.MyRandomGenerator;

public class RandomFactory
{
    public static string FirstName()
    {
        return new RandomizerFirstName(options: new FieldOptionsFirstName { Male = true }).Generate();
    }

    public static string LastName()
    {
        return new RandomizerLastName(options: new FieldOptionsLastName()).Generate();
    }

    public static string Text(int length)
    {
        return new RandomizerText(options: new FieldOptionsText
        {
            Min = length,
            Max = length,
            UseLetter = true,
            UseSpecial = false
        }).Generate();
    }

    public static string Email()
    {
        return new RandomizerEmailAddress(options: new FieldOptionsEmailAddress()).Generate();
    }
}