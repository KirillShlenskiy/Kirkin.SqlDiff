namespace Kirkin.Diff
{
    /// <summary>
    /// Output format used by diff text rendering engine.
    /// </summary>
    public enum DiffTextFormat
    {
        /// <summary>
        /// List format. Each line will contain a full
        /// description of the change including parent nodes.
        /// </summary>
        List,

        /// <summary>
        /// Tree format. Each line will only contain individual
        /// node information, appropriately indented.
        /// </summary>
        Tree
    }
}