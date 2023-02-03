using System.Dynamic;
using Adapter.JsExecutor.Utils;
using Core.Models.Executor;
using FluentAssertions;

namespace Adapter.JsExecutor.Tests.Utils;

public class JsonConversionTests
{
    [Fact]
    public void ConvertDynamicToReplica_ConversionIsCorrect()
    {
        //Arrange
        dynamic dyn = new ExpandoObject();
        dynamic answer1 = new ExpandoObject();
        answer1.id = "10";
        answer1.text = "text1";
        dynamic answer2 = new ExpandoObject();
        answer2.id = "20";
        answer2.text = "text2";

        dyn.text = "text";
        dyn.takesFreeText = true;
        dyn.answers = new ExpandoObject[]
        {
            answer1, answer2
        };
        //Act
        ReplicaModel replica = JsConversions.ConvertDynamicToReplica(dyn);
        //Assert
        Assert.Equal("text", replica.Text);
        Assert.True(replica.TakesFreeText);
        Assert.Equal(2, replica.Answers.Length);
        replica.Answers[0].Should().BeEquivalentTo(new AnswerModel { Id = "10", Text = "text1" });
        replica.Answers[1].Should().BeEquivalentTo(new AnswerModel { Id = "20", Text = "text2" });
    }
}