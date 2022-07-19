import { createContext } from 'react'

interface IThemeContext {
  themeMode: string;
  toggleTheme?: () => void
}

const defaultState = {
  themeMode: 'light'
}

export const ThemeContext = createContext<IThemeContext>(defaultState)