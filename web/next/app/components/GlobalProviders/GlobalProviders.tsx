import ThemeContextProvider from '../ThemeContextProvider/ThemeContextProvider'
import Themer from '../Themer/Themer'

type Props = {
  children: JSX.Element | JSX.Element[]
}

const GlobalProviders = ({ children }: Props) => (
  <ThemeContextProvider>
    <Themer>{children}</Themer>
  </ThemeContextProvider>
)

export default GlobalProviders
