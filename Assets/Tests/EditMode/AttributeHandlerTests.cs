using System.Collections;
using System.Collections.Generic;
using Heathside.Attributes;
using NUnit.Framework;
using NSubstitute;
using UnityEngine;
using UnityEngine.TestTools;

public class AttributeHandlerTests
{

    SingleAttribute health;
    AttributeProcessHandler handler;
    float baseValue = 10f;

    [SetUp]
    public void SetUpBeforeEachTest()
    {
        health = new SingleAttribute(baseValue);
        handler = new AttributeProcessHandler(health);
    }

    [Test]
    public void BaseProcessorValueTest()
    {
        handler.AddBaseProcessors(
            (hp) => 2f,
            (hp) => 4f,
            (hp) => 5f,
            (hp) => -10f);

        float output = handler.ProcessAll();

        Assert.AreEqual(11, output, 0.0001f);
    }

    [Test]
    public void StackProcessorsTest()
    {
        handler.StackProcessors(
            hp => hp * 1.1f,
            hp => hp * 1.2f,
            hp => hp * 1.3f,
            hp => hp * 1.25f,
            hp => hp * 0.76f,
            hp => (hp + 1) * 2f);

        float output = handler.ProcessAll();

        Assert.AreEqual(34.604f, output, 0.0001f);
    }

    [Test]
    public void StackedProcessorsWithBaseTest()
    {
        handler.AddBaseProcessors(
            (hp) => 2f,
            (hp) => 1f,
            (hp) => 3f,
            (hp) => -4f);
        handler.StackProcessors(
            (hp) => hp * 1.1f,
            (hp) => hp * 0.9f,
            (hp) => (hp + 2) / 2f);

        float output = handler.ProcessAll();

        Assert.AreEqual(7.95f, output, 0.0001f);
    }

    [Test]
    public void SeveralTimeProcessTest()
    {
        handler.AddBaseProcessors(
            (hp) => 2f,
            (hp) => 1f,
            (hp) => 3f,
            (hp) => -4f);
        handler.StackProcessors(
            (hp) => hp * 1.1f,
            (hp) => hp * 0.9f,
            (hp) => (hp + 2) / 2f);

        float output1 = handler.ProcessAll();
        float output2 = handler.ProcessAll();
        float output3 = handler.ProcessAll();

        Assert.AreEqual(output1, output2);
        Assert.AreEqual(output2, output3);
    }

    [Test]
    public void AttributeChangedTest()
    {
        handler.AddBaseProcessors(
            (hp) => 2f,
            (hp) => 1f,
            (hp) => 3f,
            (hp) => -4f);
        handler.StackProcessors(
            (hp) => hp * 1.1f,
            (hp) => hp * 0.9f,
            (hp) => (hp + 2) / 2f);

        float output1 = handler.ProcessAll();

        health.Value = 15f;
        bool changed = ReflectionUtils.GetFieldValue<bool>(handler, "changed");
        Assert.True(changed);

        float output2 = handler.ProcessAll();

        changed = ReflectionUtils.GetFieldValue<bool>(handler, "changed");
        Assert.False(changed);

        Assert.AreNotEqual(output1, output2);
        Assert.AreEqual(10.425f, output2, 0.0001f);
    }

    [Test]
    public void ListsAreEmptyAfterRemovingTest()
    {
        IAttributeProcessor[] baseProcessors = handler.AddBaseProcessors(
            (hp) => 1f,
            (hp) => 2f,
            (hp) => 3f,
            (hp) => 4f,
            (hp) => 5f);
        IAttributeProcessor[] stackProcessors = handler.StackProcessors(
            (hp) => 1f,
            (hp) => 2f,
            (hp) => 3f,
            (hp) => 4f,
            (hp) => 5f);


        handler.ProcessAll();
        handler.RemoveProcessors(baseProcessors);
        handler.RemoveProcessors(stackProcessors);

        List<IAttributeProcessor> stackList = 
            ReflectionUtils.GetFieldValue<List<IAttributeProcessor>>(handler, "stackedProcessors");
        List<IAttributeProcessor> baseList =
            ReflectionUtils.GetFieldValue<List<IAttributeProcessor>>(handler, "baseProcessors");

        Assert.True(stackList.Count == 0);
        Assert.True(baseList.Count == 0);
    }
}
