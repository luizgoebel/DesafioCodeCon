namespace DesafioCodeCon.Models;

public class Usuario
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int Age { get; set; }
    public int Score { get; set; }
    public bool Active { get; set; }
    public string Country { get; set; } = string.Empty;
    public Team Team { get; set; } = new Team();
    public List<Log> Logs { get; set; } = new List<Log>();
}

public class Team
{
    public string Name { get; set; } = string.Empty;
    public bool Leader { get; set; }
    public List<Project> Projects { get; set; } = new List<Project>();
}

public class Project
{
    public string Name { get; set; } = string.Empty;
    public bool Completed { get; set; }
}

public class Log
{
    public DateOnly Date { get; set; }
    public string Action { get; set; } = string.Empty;
}