using System.Linq.Expressions;

namespace ExpressionTrees.Task1.ExpressionsTransformer
{
    public class IncDecExpressionVisitor : ExpressionVisitor
    {
        // todo: feel free to add your code here
        protected override Expression VisitBinary(BinaryExpression node)
        {
            if (node.Left.NodeType != ExpressionType.Parameter ||
                node.Right is not ConstantExpression constant ||
                constant.Value?.ToString() != "1")
            {
                return base.VisitBinary(node);
            }

            return node.NodeType switch
            {
                ExpressionType.Add => Expression.Increment(node.Left),
                ExpressionType.Subtract => Expression.Decrement(node.Left),
                _ => base.VisitBinary(node)
            };
        }
    }
}
