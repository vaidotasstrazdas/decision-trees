using System.Collections.Generic;

namespace Bridge.IBLL.Data
{
    public class ForexDto
    {

        public string FileName { get; set; }
        public IList<ForexTreeData> ForexData { get; set; }

    }
}
