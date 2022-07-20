import { createTheme } from '@mui/material'
import { red, teal } from '@mui/material/colors'

const lightTheme = createTheme({
  palette: {
    mode: 'light',
    primary: {
      main: teal[700]
    },
    secondary: {
      main: red[400]
    }
  }
})

export default lightTheme
