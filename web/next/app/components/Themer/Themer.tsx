import { ThemeProvider } from '@mui/material'
import { useContext } from 'react'
import { ThemeContext } from '../../lib/context/ThemeContext'
import darkTheme from '../../styles/theme/darkTheme'
import lightTheme from '../../styles/theme/lightTheme'

type Props = {
  children: JSX.Element | JSX.Element[]
}

const Themer = ({ children }: Props) => {
  const { themeMode } = useContext(ThemeContext)
  
  return (
    <ThemeProvider theme={themeMode === 'light' ? lightTheme : darkTheme}>
      {children}
    </ThemeProvider>
  )
}

export default Themer