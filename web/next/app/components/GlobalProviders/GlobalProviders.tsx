import { Session } from 'next-auth'
import { SessionProvider } from 'next-auth/react'
import ThemeContextProvider from '../ThemeContextProvider/ThemeContextProvider'
import Themer from '../Themer/Themer'

type Props = {
  session: Session
  children: JSX.Element | JSX.Element[]
}

const GlobalProviders = ({ session, children }: Props) => (
  <SessionProvider session={session}>
    <ThemeContextProvider>
      <Themer>{children}</Themer>
    </ThemeContextProvider>
  </SessionProvider>
)

export default GlobalProviders
