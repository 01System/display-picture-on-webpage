<table id="ListType" runat="server" class="table1">
  <tr>
    <th class="td_Content" style="width:150px">
       <%# bp_GetLang("Picture")%>
    </th>
  </tr>
</table>
<table id="CopyListType" style="display:none">
  <tr>
     <td>
        <img ID="img"/>
     </td>
  </tr>
</table>


<script type="text/javascript">
   function DisplayPicture(id)
   {
      document.getElementById("img").src="this is path of ShowPicture page in Controller folder "
   }
<script>
