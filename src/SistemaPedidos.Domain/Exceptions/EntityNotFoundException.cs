namespace SistemaPedidos.Domain.Exceptions;

public sealed class EntityNotFoundException(string message) : Exception(message);
