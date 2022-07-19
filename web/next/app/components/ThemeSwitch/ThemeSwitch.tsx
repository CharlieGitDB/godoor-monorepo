import { NightsStay, WbSunny } from '@mui/icons-material'
import { Fade } from '@mui/material'
import { useContext } from 'react'
import { ThemeContext } from '../../lib/context/ThemeContext'

const ThemeSwitch = () => {
  const { themeMode, toggleTheme } = useContext(ThemeContext)

  return (
    <>
      <span onClick={toggleTheme} className="cursor-pointer">
        {themeMode === 'dark' ? <Fade in={true}><WbSunny /></Fade> : <NightsStay />}
      </span>
    </>
  )
}

export default ThemeSwitch