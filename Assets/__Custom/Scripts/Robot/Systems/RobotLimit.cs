namespace Hackcreeper.LD54.Robot.Systems
{
    public class RobotLimit
    {
        #region EXPOSED FIELDS

        public int MaxStructureModules { get; private set; } = 5;
        public int MaxModulePoints { get; private set; } = 16;
        public int MaxLogicModules { get; private set; } = 2;

        #endregion
    }
}