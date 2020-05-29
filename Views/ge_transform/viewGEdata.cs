using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authorization;
using ge_repository.Models;

namespace ge_repository.Pages
{
    [AllowAnonymous]
    public class viewGEdata : PageModel
    {
 		private readonly ge_DbContext _context;

		protected string xslfolder = "xsl_stylesheets";
		protected string xslfile = "";
		protected string xml_data = "";
        public viewGEdata (ge_DbContext context)
        {
            _context = context;
        }
/*   	public async Task<IActionResult> OnGetAsync(Guid? id, string holeId, string view)
        {
		try {
            if (id == null)
            {
                return NotFound();
            }

            var ge_data = await _context.ge_data.
                .Include(g => g.owner)
                .Include(g => g.project).FirstOrDefaultAsync(m => m.Id == id);

            if (ge_data == null)
            {
                return NotFound();
            }
           ViewData["ownerId"] = new SelectList(_context.Set<ge_user>(), "Id", "Id");
           ViewData["projectId"] = new SelectList(_context.ge_project, "Id", "Id");


			addMessage("Parameters passsed (id=[" + id + "];holeid=[" + holeid + "];view=[" + view + "])");
		
			if (get_xslfilename(id, view)) {
				addMessage ("Using xslt data transform stylsheet: " + xslfile);
					if (get_borehole(id, holeId)) {
						addMessage ("xml_data returned: " + xml_data.Length + " char(s)");
							Xml1.DocumentContent = xml_data;
							Xml1.TransformSource = Server.MapPath(xslfolder + "/" + xslfile);
					} else {
						addMessage ("No xml data returned");
					}
				} else {
				addMessage ("No valid stylesheet was found for the selected view type and xml data set; check the version of the ags data and the type of xml data structure");
				}
		}
		catch (Exception ex) {
			addMessage (ex.ToString());
			return Page();
		}
	}

	private bool  get_xslfilename( int id, string stylename) {
			try {	
				
				SqlConnection con = ConnectionManager.GetGroundEngineeringDataConnection();
				SqlCommand cmd = new SqlCommand("sp_getStylesheetFileName", con);
				
				cmd.CommandType = CommandType.StoredProcedure;
				
				cmd.Parameters.Add("@data_id", SqlDbType.Int).Value = id;
				
				if (String.IsNullOrEmpty(stylename) != true) {
					cmd.Parameters.Add("@stylename", SqlDbType.VarChar).Value = stylename;
				}
				
				SqlParameter output_param = cmd.Parameters.Add("@filename", SqlDbType.VarChar, 64);
				output_param.Direction = ParameterDirection.Output;
				
				con.Open();
				
				cmd.ExecuteNonQuery();
				
				xslfile = Convert.ToString(output_param.Value);
				
				return true;
				
			}
			catch (Exception ex) {
				addMessage(ex.ToString());
				return false ;
			}
	}

	private bool get_borehole( Guid id, string holeid) {
		try {	
			
			SqlConnection con = ConnectionManager.GetGroundEngineeringDataConnection();
			SqlCommand cmd = new SqlCommand("sp_getBorehole", con);
			
			cmd.CommandType = CommandType.StoredProcedure;
			cmd.Parameters.Add("@data_id", SqlDbType.Int).Value = id;
			cmd.Parameters.Add("@hole_id", SqlDbType.VarChar).Value = holeid;
			
			SqlParameter output_param = cmd.Parameters.Add("@outputXML", SqlDbType.Xml);
			output_param.Direction = ParameterDirection.Output;
			
			con.Open();
			
			cmd.ExecuteNonQuery();
			
			xml_data = Convert.ToString(output_param.Value);
			
			return true;	
		}
		catch (Exception ex) {
			addMessage (ex.ToString() );
			return false;
		}
    }
	

		
	private void addMessage(string s1) {
			TextBox1.Text += s1 + "\r\n";
	}
	} */

}

}

       
 