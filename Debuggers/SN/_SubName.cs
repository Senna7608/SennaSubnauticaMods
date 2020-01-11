namespace Debuggers.SN
{
    public static class _SubName
    {
        public static void DebugSubName(this SubName subName, string prefixString = null)
        {
            if (prefixString == null)
            {
                DLog.Log($"\n[SubName]");
                prefixString = "this";
            }

            DLog.Log($"{prefixString}.enabled: {subName.enabled}");
            DLog.Log($"{prefixString}.gameObject: {subName.gameObject.name}");
            DLog.Log($"{prefixString}.hideFlags: {subName.hideFlags}");
            DLog.Log($"{prefixString}.hideFlags: {subName}");


            DLog.Log($"{prefixString}.tag: {subName.tag}");
            DLog.Log($"{prefixString}.tag: {subName.transform.name}");
            

            
        }




    }
}
