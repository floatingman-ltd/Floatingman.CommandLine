using FluentAssertions;
using Moq;
using Xunit;

namespace CaseWare.CommandLineParser.xUnit
{
    public class CommandLineTests
    {
        [Fact]
        public void Parse_accepts_commands()
        {
            // This is a sample of what the test would look like if the input data were mocked.
            // var args = new string[] { "azalea" };

            var args = new Mock<string[]>();
            args.Setup(a => a[0]).Returns("azalea");

            var parameters = CommandLine.Instance.Parse<CommandTestArgs>(args.Object);

            parameters.Command.Should().Be("azalea");
        }

        [Fact]
        public void Parse_positional_argument_can_be_after_boolean_options()
        {
            var args = new string[] { "-e", "123", "shark attack" };

            var parameters = CommandLine.Instance.Parse<TestArgs>(args);

            parameters.Shark.Should().Be("shark attack");
        }

        [Fact]
        public void Parse_positional_argument_can_be_after_value_options()
        {
            var args = new string[] { "-a", "shark attack" };

            var parameters = CommandLine.Instance.Parse<TestArgs>(args);

            parameters.Shark.Should().Be("shark attack");
        }

        [Fact]
        public void Parse_sets_longForm_bool_to_true()
        {
            var args = new string[] { "--dog" };

            var parameters = CommandLine.Instance.Parse<TestArgs>(args);

            parameters.Dog.Should().BeTrue();
        }

        [Fact]
        public void Parse_sets_shortForm_bool_to_true()
        {
            var args = new string[] { "-b" };

            var parameters = CommandLine.Instance.Parse<TestArgs>(args);

            parameters.Baboon.Should().BeTrue();
        }

        [Fact]
        public void Parse_should_get_an_array_of_double()
        {
            var args = new string[] { "-i", "1.23,4.56" };

            var parameters = CommandLine.Instance.Parse<TestArgs>(args);

            parameters.Ibis.Should().Equal(1.23, 4.56);
        }

        [Fact]
        public void Parse_should_get_an_array_of_int()
        {
            var args = new string[] { "-h", "123,456,789" };

            var parameters = CommandLine.Instance.Parse<TestArgs>(args);

            parameters.Hog.Should().Equal(123, 456, 789);
        }

        [Fact]
        public void Parse_should_get_an_array_of_string()
        {
            var args = new string[] { "-j", "there,are,dogs,about" };

            var parameters = CommandLine.Instance.Parse<TestArgs>(args);

            parameters.Jaguar.Should().Equal("there", "are", "dogs", "about");
        }

        [Fact]
        public void Parse_should_get_an_double()
        {
            var args = new string[] { "--frog", "123.456" };

            var parameters = CommandLine.Instance.Parse<TestArgs>(args);

            parameters.Frog.Should().Be(123.456);
        }

        [Fact]
        public void Parse_should_get_an_int()
        {
            var args = new string[] { "-e", "123" };

            var parameters = CommandLine.Instance.Parse<TestArgs>(args);

            parameters.Elephant.Should().Be(123);
        }

        [Fact]
        public void Parse_should_get_an_string()
        {
            var args = new string[] { "-g", "will eat anything" };

            var parameters = CommandLine.Instance.Parse<TestArgs>(args);

            parameters.Goat.Should().Be("will eat anything");
        }

        [Fact]
        public void Parse_should_get_argument()
        {
            var args = new string[] { "shark attack" };

            var parameters = CommandLine.Instance.Parse<TestArgs>(args);

            parameters.Shark.Should().Be("shark attack");
        }

        [Fact]
        public void Parse_should_get_two_arguments()
        {
            var args = new string[] { "shark attack", "123" };

            var parameters = CommandLine.Instance.Parse<TestArgs>(args);

            parameters.Shark.Should().Be("shark attack");
            parameters.Tiger.Should().Be(123);
        }

        [Fact]
        public void Parse_should_log_an_error_if_unknown_parameter_passed_in()
        {
            var args = new string[] { "--zoo" };

            var parameters = CommandLine.Instance.Parse<TestArgs>(args);

            parameters.Errors.Should().NotBeEmpty();
            parameters.Errors[0].Should().Be("zoo is not recognized");
        }

        [Fact]
        public void Parse_should_pick_up_single_items_as_IEnumerable()
        {
            var args = new string[] { "-j", "shark attack" };

            var parameters = CommandLine.Instance.Parse<TestArgs>(args);

            parameters.Jaguar.Should().Equal("shark attack");
        }

        [Fact]
        public void Parse_should_return_Default_value_if_no_value_set()
        {
            var args = new string[0];

            var parameters = CommandLine.Instance.Parse<TestArgs>(args);

            parameters.Kangaroo.Should().Be(555);
        }

        [Fact]
        public void Parse_should_return_error_list_if_IsRequired_and_Default_and_no_value()
        {
            var args = new string[0];

            var parameters = CommandLine.Instance.Parse<TestRequiredArgs>(args);

            parameters.Errors.Should().NotBeEmpty();
            parameters.Errors[0].Should().Be("Airplane is required, but not set");
        }

        [Theory]
        [InlineData("-c")]
        [InlineData("--cat")]
        public void Parse_will_accept_either_long_or_short_form(params string[] args)
        {
            var parameters = CommandLine.Instance.Parse<TestArgs>(args);

            parameters.Cat.Should().BeTrue();
        }

        [Fact]
        public void Parse_will_log_an_error_if_multiple_parameters_have_same_short_option()
        {
            var args = new string[] { "-b", "-b" };

            var parameters = CommandLine.Instance.Parse<TestArgs>(args);

            parameters.Errors.Should().NotBeEmpty();
            parameters.Errors[0].Should().Be("b is duplicated");
        }

        [Fact]
        public void Parse_will_not_honor_IsRequired_on_bool()
        {
            var args = new string[0];

            var parameters = CommandLine.Instance.Parse<TestArgs>(args);

            parameters.Aardvark.Should().BeFalse();
        }

        [Theory]
        [InlineData("-e", "eating peanuts")]
        [InlineData("-e", "3.14")]
        [InlineData("--frog", "sitting on a lily pad")]
        [InlineData("-h", "1,1.21")]
        public void Parse_will_return_error_list_if_data_not_coercable(params string[] args)
        {
            var parameters = CommandLine.Instance.Parse<TestArgs>(args);

            parameters.Errors.Should().NotBeEmpty();
            parameters.Errors[0].Should().StartWith($"[{args[1]}] cannot be converted to");
        }

        [Fact]
        public void Parse_will_throw_an_exception_if_multiple_parameters_have_same_long_option()
        {
            var args = new string[] { "--cat", "--cat" };

            var parameters = CommandLine.Instance.Parse<TestArgs>(args);

            parameters.Errors.Should().NotBeEmpty();
            parameters.Errors[0].Should().Be("cat is duplicated");
        }

        [Fact]
        public void Parse_will_throw_an_exception_if_multiple_parameters_have_same_option()
        {
            var args = new string[] { "-c", "--cat" };

            var parameters = CommandLine.Instance.Parse<TestArgs>(args);

            parameters.Errors.Should().NotBeEmpty();
            parameters.Errors[0].Should().Be("cat is duplicated");
        }

        [Command("plants")]
        private class CommandTestArgs : CommandArgs
        {
        }

        private class TestArgs : CommandArgs
        {
            // options
            [Option('a', IsRequired = true)] public bool Aardvark { get; set; }

            [Option('b')] public bool Baboon { get; set; }

            [Option('c', "cat")] public bool Cat { get; set; }

            [Option("dog")] public bool Dog { get; set; }

            [Option('e')] public int Elephant { get; set; }

            [Option("frog")] public double Frog { get; set; }

            [Option('g')] public string Goat { get; set; }

            [Option('h')] public int[] Hog { get; set; }

            [Option('i')] public double[] Ibis { get; set; }

            [Option('j')] public string[] Jaguar { get; set; }

            [Option('k', Default = 555)] public int Kangaroo { get; set; }

            // arguments
            [Argument(0)] public string Shark { get; set; }

            [Argument(1)] public int Tiger { get; set; }
        }

        private class TestRequiredArgs : CommandArgs
        {
            [Option("airplane", Default = 777, IsRequired = true, Name = "Airplane")] public int Airplane { get; set; }
        }
    }
}