namespace SistemaPedidos.Domain.Exceptions;

public sealed class ConcurrencyConflictException(string message) : Exception(message);
