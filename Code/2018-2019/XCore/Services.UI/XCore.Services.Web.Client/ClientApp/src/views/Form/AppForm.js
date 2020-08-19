import React, { Component, Fragment} from 'react';
import TextField from '@material-ui/core/TextField';
import { makeStyles } from '@material-ui/core/styles';
import { useStyles_Form } from '../../helpers/Utilties/MaterialUIStyles';
/*
function HandleChange(evt) {
    const value = evt.target.value;
    this.setState({
     
      [evt.target.name]: value
    });
  }
  function HandleClick()
  {
      console.log("Form Input State",this.state);
  }
  const [{APPID,APPName,APPCode,ModuleID,ModuleName,ModuleCode,ModuleConfigID,ModuleConfigName,ModuleConfigCode},HandleChange] = useState('');

*/
export default function AppForm()
{
debugger;
  // Declare a new state variable, which we'll call "count"
 
  const classes=useStyles_Form();
  console.log(classes);
        return (

            <form className={classes.container} noValidate autoComplete="off">
            <div>
            <TextField
                id="standard-basic"
                className={classes.textField}
                label="AppID"
                name="AppID"
                margin="normal"
               
            />
            </div>
            <div>
            <TextField
                id="filled-basic"
                className={classes.textField}
                label="AppName"
                name="AppName"
                margin="normal"
             
              
            />
            </div>
            <div>
            <TextField
                id="outlined-basic"
                className={classes.textField}
                label="AppCode"
                name="AppCode"
                margin="normal"
              
              
            />
            </div>

            <div>
            <TextField
                id="standard-basic"
                className={classes.textField}
                label="ModuleID"
                name="ModuleID"
                margin="normal"
               
            />
            </div>
            <div>
            <TextField
                id="filled-basic"
                className={classes.textField}
                label="ModuleName"
                name="ModuleName"
                margin="normal"
               
              
            />
            </div>
            <div>
            <TextField
                id="outlined-basic"
                className={classes.textField}
                label="ModuleCode"
                name="ModuleCode"
                margin="normal"
              
            />
            </div>


            <div>
            <TextField
                id="standard-basic"
                className={classes.textField}
                label="ModuleConfigID"
                name="ModuleConfigID"
                margin="normal"
               
            />
            </div>
            <div>
            <TextField
                id="filled-basic"
                className={classes.textField}
                label="ModuleConfigName"
                name="ModuleConfigName"
                margin="normal"
           
            />
            </div>
            <div>
            <TextField
                id="outlined-basic"
                className={classes.textField}
                label="ModuleConfigCode"
                name="ModuleConfigCode"
                margin="normal"
         
              
            />
            </div>

        </form>



        
        )
    }

