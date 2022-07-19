import { ReactChild, ReactChildren, useState } from 'react'
import { ThemeContext } from '../../lib/context/ThemeContext'
import { ThemeMode } from '../../types/ThemeMode'

type Props = {
  children: ReactChild | ReactChildren
}

const ThemeContextProvider = ({ children }: Props) => {
  const [themeMode, setThemeMode] = useState<ThemeMode>('light')
  const toggleTheme = () => setThemeMode(themeMode === 'light' ? 'dark' : 'light')

  return (
    <>
      <ThemeContext.Provider value={{themeMode, toggleTheme}}>
        {children}
      </ThemeContext.Provider>
    </>
  )
}

export default ThemeContextProvider