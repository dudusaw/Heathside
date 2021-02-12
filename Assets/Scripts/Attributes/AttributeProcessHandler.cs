using System.Collections.Generic;
using System;

namespace Heathside.Attributes
{
    public interface IAttributeProcessor
    {
        float Process(float input);
    }

    public sealed class AttributeProcessHandler
    {
        private sealed class ProcessWrapper : IAttributeProcessor
        {
            private readonly Func<float, float> function;

            public ProcessWrapper(Func<float, float> function)
            {
                this.function = function;
            }

            public float Process(float input)
            {
                return function(input);
            }
        }

        private bool changed = true;
        private float output;
        private readonly SingleAttribute attribute;
        private readonly List<IAttributeProcessor> stackedProcessors;
        private readonly List<IAttributeProcessor> baseProcessors;

        public AttributeProcessHandler(SingleAttribute attribute)
        {
            this.attribute = attribute;
            attribute.OnChanged += Changed;
            stackedProcessors = new List<IAttributeProcessor>();
            baseProcessors = new List<IAttributeProcessor>();
        }

        private void Changed()
        {
            changed = true;
        }

        public void Dispose()
        {
            attribute.OnChanged -= Changed;
        }

        public void StackProcessors(params IAttributeProcessor[] processors)
        {
            stackedProcessors.AddRange(processors);
            changed = true;
        }

        public void AddBaseProcessors(params IAttributeProcessor[] processors)
        {
            baseProcessors.AddRange(processors);
            changed = true;
        }

        public IAttributeProcessor[] StackProcessors(params Func<float, float>[] processors)
        {
            return AddProcessorsTo(stackedProcessors, processors);
        }

        public IAttributeProcessor[] AddBaseProcessors(params Func<float, float>[] processors)
        {
            return AddProcessorsTo(baseProcessors, processors);
        }

        private IAttributeProcessor[] AddProcessorsTo(List<IAttributeProcessor> list, Func<float, float>[] processors)
        {
            ProcessWrapper[] resultList = new ProcessWrapper[processors.Length];
            for (int i = 0; i < processors.Length; i++)
            {
                resultList[i] = new ProcessWrapper(processors[i]);
            }
            list.AddRange(resultList);
            changed = true;
            return resultList;
        }

        public void RemoveProcessors(params IAttributeProcessor[] processors)
        {
            foreach (var processor in processors)
            {
                changed |= stackedProcessors.Remove(processor);
                changed |= baseProcessors.Remove(processor);
            }
        }

        public float ProcessAll()
        {
            if (!changed)
            {
                return output;
            }
            float processedValue = attribute.Value;
            for (int i = 0; i < stackedProcessors.Count; i++)
            {
                processedValue = stackedProcessors[i].Process(processedValue);
            }
            for (int i = 0; i < baseProcessors.Count; i++)
            {
                processedValue += baseProcessors[i].Process(attribute.Value);
            }
            changed = false;
            output = processedValue;
            return processedValue;
        }
    }
}