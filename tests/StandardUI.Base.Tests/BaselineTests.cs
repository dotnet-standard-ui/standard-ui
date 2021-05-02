using Microsoft.StandardUI.Base.Tests.Mocks;
using Microsoft.StandardUI.Drawing;
using Microsoft.StandardUI.Elements;
using Microsoft.StandardUI.Tree;
using System.Collections.Generic;
using Xunit;
using static Microsoft.StandardUI.Elements.Factories;

namespace Microsoft.StandardUI.Base.Tests
{
    public class BaselineTests
    {
        Context context;

        public BaselineTests()
        {
            StateManager stateManager = new(_ => { });
            context = new(stateManager, new Layer(), new());
        }

        [Fact]
        public void HStackBaseline()
        {
            var element = HStack(VerticalAlignment.Baseline,
                new SetBaseline(Rectangle(Colors.Red, 10, 100), 0),
                Rectangle(Colors.Blue, 10, 100));
            var node = element.CreateNode(null, context);
            var (size, baseline) = node.Arrange(new(500, 500));
            Assert.Equal(new(20, 200), size);
            Assert.Equal(100, baseline);
        }

        [Fact]
        public void RowBaseline_SkipTop()
        {
            var element = HStack(VerticalAlignment.Baseline,
                new SetBaseline(Rectangle(Colors.Red, 10, 100), 0).Top(),
                Rectangle(Colors.Blue, 10, 100));
            var node = element.CreateNode(null, context);
            var (size, baseline) = node.Arrange(new(500, 500));
            Assert.Equal(new(20, 100), size);
            Assert.Equal(100, baseline);
        }

        [Fact]
        public void ColumnBaseline()
        {
            var element = VStack(
                Rectangle(Colors.Red, 10, 10),
                new SetBaseline(Rectangle(Colors.Green, 10, 10), 5),
                Rectangle(Colors.Blue, 10, 10));
            var node = element.CreateNode(null, context);
            var (size, baseline) = node.Arrange(new(500, 500));
            Assert.Equal(new(10, 30), size);
            Assert.Equal(15, baseline);
        }

        internal class SetBaseline : Element
        {
            public SetBaseline(Element child, float? baseline)
            {
                Child = child;
                Baseline = baseline;
            }

            public Element Child { get; }
            public float? Baseline { get; }

            public override Node CreateNode(Node? parent, Context context) =>
                new SetBaselineNode(parent, context, this);
        }

        internal class SetBaselineNode : NodeBase<SetBaseline>
        {
            Node child;

            public SetBaselineNode(Node? parent, Context context, SetBaseline element) : base(parent, context, element)
            {
                child = element.Child.CreateNode(this, context);
            }

            public override IEnumerable<Node> Children
            {
                get
                {
                    yield return child;
                }
            }

            protected override (Size, float?) ArrangeOverride(Size availableSize)
            {
                var (size, _) = child.Arrange(availableSize);
                return (size, Element.Baseline);
            }

            protected override void UpdateElement(SetBaseline oldElement, Context oldContext)
            {
                if (oldElement.Baseline != Element.Baseline)
                    Context.InvalidateLayout();
                child = child.UpdateElement(Element.Child, Context);
            }
        }
    }
}
