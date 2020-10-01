import React from 'react';
import { makeStyles } from '@material-ui/core/styles';
import AppBar from '@material-ui/core/AppBar';
import Toolbar from '@material-ui/core/Toolbar';
import IconButton from '@material-ui/core/IconButton';
import MenuIcon from '@material-ui/icons/Menu';

// import 'typeface-roboto';
const useStyles = makeStyles((theme) => ({
  root: {
    flexGrow: 1,
    paddingBottom: 8,
  },
  menuButton: {
    marginRight: theme.spacing(2),

  },
  appBar: {
    backgroundColor: '#483D8B',
    boxShadow: theme.shadows[10]
  },

}));

export default function Header() {
  const classes = useStyles();

  return (
    <div className={classes.root}>
      <AppBar position="static" className={classes.appBar}>
        <Toolbar variant="dense">
          <IconButton edge="start" className={classes.menuButton} color="inherit" aria-label="menu">
            <MenuIcon />
          </IconButton>
        </Toolbar>
      </AppBar>
    </div>
  );
}