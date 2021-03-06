﻿using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Data.OracleClient;
using System.Configuration;
using System.Reflection;
using System.Collections;

namespace DAL
{
    protected abstract class Connection
    {
        //get connection object from configration file
        static string str = "data source='???';user id='???';password='???'";
        protected string ConnectionString { get; set; }
        OracleCommand cmd; //get data command   
        private OracleConnection conn; //get data connection

        /// <summary>
        /// get connection string form Web.Config
        /// </summary>
        /// <param name="Name"></param>
        /// <returns></returns>
        protected string GetConfig(string Name)
        {
            string CONFIGNAME = System.Configuration.ConfigurationManager.ConnectionStrings[Name].ToString();
            return CONFIGNAME;
        }
        //the property of connect to database
        protected OracleConnection Conn
        {
            get
            {
                if (conn == null)
                {
                    conn = new OracleConnection(ConnectionString);
                    conn.Open();
                } if (conn.State == ConnectionState.Closed)
                {
                    conn.Open();
                } if (conn.State == ConnectionState.Broken)
                {
                    conn.Close();
                    conn.Open();
                }
                return conn;
            }
        }

        //close data connection
        protected void close()
        {
            Conn.Close();
        }

        /// <summary>
        /// execute command, use for transform Blob to Byte in database
        /// </summary>
        /// <param name="cmd">the parameters of command statement</param>
        /// <returns>byte format binary stream</returns>
        protected byte[] BlobToByte(OracleCommand cmd)
        {
            long blobDataSize = 0;//BLOD full-size
            long readStartByte = 0;//where begin to write on Blod data
            int bufferStartByte = 0;// where begin to write on buffer array
            int hopeReadSize = 1024;//expected the size of every time read form BLOB
            long realReadSize = 0;// the actual size of every time read from BLOB 

            //CommandBehavior.SequentialAccess make OracleDataReader load BLOB data as stream
            try
            {
                cmd.Connection = Conn;
                OracleDataReader dr = cmd.ExecuteReader(CommandBehavior.SequentialAccess);
                byte[] buffer = null;
                if (dr.Read())
                {
                    blobDataSize = dr.GetBytes(0, 0, null, 0, 0); //get the BLOB data of total size
                    buffer = new byte[blobDataSize];
                    realReadSize = dr.GetBytes(0, readStartByte, buffer, bufferStartByte, hopeReadSize);
                    while ((int)realReadSize == hopeReadSize)//use for cyclic reading 1024bytes
                    {
                        bufferStartByte += hopeReadSize;
                        readStartByte += realReadSize;
                        realReadSize = dr.GetBytes(0, readStartByte, buffer, bufferStartByte, hopeReadSize);
                    }
                    //read the rest of data which are less than 1024 bytes from BLOB
		    //after reading complete , the BLOB data transform to buffer as binary
                    dr.GetBytes(0, readStartByte, buffer, bufferStartByte, (int)realReadSize);
                   
                }
		else 
		{
			buffer = null;
		}
                return buffer;
            }
            catch (OracleException ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                this.close();
            }

        }
    }
}
