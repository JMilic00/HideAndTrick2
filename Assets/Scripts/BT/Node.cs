using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace Pathfinding.BehaviourTrees
{
    public class Node
    {
        public enum Status { Success, Failure, Running }

        public readonly string name;

        public readonly List<Node> children = new();
        protected int currentChild;

        public Node(string name = "Node")
        {
            this.name = name;
        }

        public void AddChild(Node child) => children.Add(child);

        public virtual Status Process() => children[currentChild].Process();

        public virtual void Reset()
        {
            currentChild = 0;
            foreach(var child in children)
            {
                child.Reset();
            }
        }
    }

    public class Leaf : Node
    {
        readonly IStrategy strategy;

        public Leaf(string name, IStrategy strategy) : base(name)
        {
            this.strategy = strategy;
        }

        public override Status Process() => strategy.Process();
    }

    public class BehaviourTree : Node
    {
        public BehaviourTree(string name) : base(name) { }

        public override Status Process()
        {
            while (currentChild < children.Count)
            {
                var status = children[currentChild].Process();
                if (status != Status.Success)
                {
                    return status;
                }
                currentChild++;
            }

            Reset();
            return Status.Success;
        }

        public override void Reset()
        {
            // Reset currentChild and all child nodes
            base.Reset();
            currentChild = 0;
        }
    }

    public class BehaviourSequence : Node
    {
        public BehaviourSequence(string name) : base(name) { }

        public override Status Process()
        {
            while (currentChild < children.Count)
            {
                var status = children[currentChild].Process();
                switch (status)
                {
                    case Status.Running:
                        return Status.Running;
                    case Status.Failure:
                        Reset();
                        return Status.Failure;
                    default:
                        currentChild++;
                        return currentChild == children.Count ? Status.Success : Status.Running;
                }
            }
            Reset();
            return Status.Success;
        }
    }

    public class BehaviourSelector : Node
    {
        public BehaviourSelector(string name) : base(name) { }

        public override Status Process()
        {
            foreach (var child in children)
            {
                var status = child.Process();
                switch (status)
                {
                    case Status.Running:
                        return Status.Running;
                    case Status.Success:
                        Reset();
                        return Status.Success;
                    default:
                        break;
                }
            }
            Reset();
            return Status.Failure;
        }
    }

    public class RandomSelector : Node
    {
        private readonly System.Random random = new System.Random();

        public RandomSelector(string name) : base(name) { }

        public override Status Process()
        {
            if (currentChild == 0) // Shuffle only once before we start processing
            {
                ShuffleChildren();
            }

            while (currentChild < children.Count)
            {
                var status = children[currentChild].Process();
                switch (status)
                {
                    case Status.Running:
                        return Status.Running;
                    case Status.Success:
                        Reset();
                        return Status.Success;
                    default:
                        currentChild++;
                        break;
                }
            }

            Reset();
            return Status.Failure;
        }

        private void ShuffleChildren()
        {
            int n = children.Count;
            while (n > 1)
            {
                n--;
                int k = random.Next(n + 1);
                Node value = children[k];
                children[k] = children[n];
                children[n] = value;
            }
        }

        public override void Reset()
        {
            base.Reset();
            currentChild = 0;
        }
    }
}
