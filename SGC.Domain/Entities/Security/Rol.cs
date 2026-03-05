public class Rol
{
    public int Id { get; set; }
    public string Nombre { get; set; }
}

public class UsuarioRol
{
    public int UsuarioId { get; set; }
    public int RolId { get; set; }
}