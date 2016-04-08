using DiffPlex;
using DiffPlex.DiffBuilder;
using DiffPlex.DiffBuilder.Model;

using Xunit;
using Xunit.Abstractions;

namespace Kirkin.SqlDiff.Tests
{
    public class DiffPlexTests
    {
        private ITestOutputHelper Output;

        public DiffPlexTests(ITestOutputHelper output)
        {
            Output = output;
        }

        private const string OldText =
            @"We the people
of the united states of america
establish justice
ensure domestic tranquility
provide for the common defence
secure the blessing of liberty
to ourselves and our posterity
";

        private const string NewText =
            @"We the people
in order to form a more perfect union
establish justice
ensure domestic tranquility
promote the general welfare and
secure the blessing of liberty
to ourselves and our posterity
do ordain and establish this constitution
for the United States of America
";

        [Fact]
        public void InlineDiff()
        {
            var d = new Differ();
            var inlineBuilder = new InlineDiffBuilder(d);
            var result = inlineBuilder.BuildDiffModel(OldText, NewText);
            foreach (var line in result.Lines)
            {
                if (line.Type == ChangeType.Inserted)
                    Output.WriteLine("+ " + line.Text);
                else if (line.Type == ChangeType.Deleted)
                    Output.WriteLine("- " + line.Text);
                else
                    Output.WriteLine("  " + line.Text);
            }
        }

        [Fact]
        public void SideBySideDiff()
        {
            var d = new Differ();
            var inlineBuilder = new SideBySideDiffBuilder(d);
            var result = inlineBuilder.BuildDiffModel(OldText, NewText);

            foreach (var line in result.OldText.Lines)
            {
                Output.WriteLine($"{line.Type}: {line.Text}");
            }

            Output.WriteLine("");

            foreach (var line in result.NewText.Lines)
            {
                Output.WriteLine($"{line.Type}: {line.Text}");
            }
        }
    }
}