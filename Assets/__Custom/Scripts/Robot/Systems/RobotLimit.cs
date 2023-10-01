namespace Hackcreeper.LD54.Robot.Systems
{
    public class RobotLimit
    {
        #region EXPOSED FIELDS

        public int MaxStructureModules { get; private set; } = 5;
        public int MaxModulePoints { get; private set; } = 3;

        #endregion
    }
}