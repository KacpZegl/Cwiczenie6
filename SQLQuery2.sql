SELECT SUM(op.Amount * op.Price) AS OrderTotal
FROM OrderPositions op
WHERE op.OrderID = 12;
