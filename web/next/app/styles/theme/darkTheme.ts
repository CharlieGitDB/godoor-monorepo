import { createTheme } from "@mui/material"
import { red, teal } from "@mui/material/colors"

const darkTheme = createTheme({
  palette: {
    mode: 'dark',
    primary: {
      main: teal[300]
    },
    secondary: {
      main: red[200]
    }
  }
})

export default darkTheme