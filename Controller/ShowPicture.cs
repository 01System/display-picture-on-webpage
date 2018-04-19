using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class ShowPicture:System.Web.UI.Page
{
	protected void Page_Load(object sender,EventArgs e)
	{
		ShowImage();
	}

	//use for display picture
	private void ShowImage()
	{
		string id = this.Request["id"];  //get id from web
		byte[] streamByte = new DAL.ShowImage().ShowImage(id);
		Response.ContentType = "image/*";//display picture format
		Response.BinaryWrite(streamByte);//paint the picture
	}
}
