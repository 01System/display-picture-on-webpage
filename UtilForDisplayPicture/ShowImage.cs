using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UtilForDisplayPicture
{
    protected class ShowImage:Connection
    {
	    public ShowImage()
	    {
		    this.ConnectionString = this.GetConfig("???");
	    }

	    // use for display picture
	    public byte[] ShowImage(string DtypeID)
	    {
		    string oraSql = "SELECT PIC FROM table_name WHERE id = "+DtypeID;
		    OracleCommand cmd = new OracleCommand(oraSql);
		    byte[] imageBytes = this.BlobToByte(cmd);
		    return imageBytes;
	    }
    }
}
