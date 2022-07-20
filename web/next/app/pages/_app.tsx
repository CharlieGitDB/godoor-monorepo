import { EmotionCache } from '@emotion/cache'
import { CacheProvider } from '@emotion/react'
import { CssBaseline } from '@mui/material'
import { Session } from 'next-auth'
import type { AppProps } from 'next/app'
import Head from 'next/head'
import GlobalProviders from '../components/GlobalProviders/GlobalProviders'
import '../styles/globals.css'
import createEmotionCache from '../util/createEmotionCache'

const clientSideEmotionCache = createEmotionCache()

interface MyAppProps extends AppProps {
  emotionCache?: EmotionCache
  session: Session
}

const MyApp = (props: MyAppProps) => {
  const {
    Component,
    emotionCache = clientSideEmotionCache,
    pageProps,
    session
  } = props

  return (
    <CacheProvider value={emotionCache}>
      <Head>
        <meta name="viewport" content="initial-scale=1, width=device-width" />
      </Head>

      <GlobalProviders session={session}>
        <CssBaseline />
        <Component {...pageProps} />
      </GlobalProviders>
    </CacheProvider>
  )
}

export default MyApp
