namespace zephyr.CLI;

using SVGAIITerminal = SVGAIITerminal.SVGAIITerminal;

public abstract class Script
{
    public string Name;
    public string Description;

    public Script(string Name, string Description)
    {
        this.Name = Name;
        this.Description = Description;
    }

    public abstract void Invoke(SVGAIITerminal Console, string[] Args);
}