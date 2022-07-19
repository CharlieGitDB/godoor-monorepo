import { EmotionCache } from '@emotion/cache'
import { CacheProvider } from '@emotion/react'
import { CssBaseline } from '@mui/material'
import type { AppProps } from 'next/app'
import Head from 'next/head'
import Themer from '../components/Themer/Themer'
import '../styles/globals.css'
import createEmotionCache from '../util/createEmotionCache'

const clientSideEmotionCache = createEmotionCache()

interface MyAppProps extends AppProps {
  emotionCache?: EmotionCache;
}

const MyApp = (props: MyAppProps) => {
  const { Component, emotionCache = clientSideEmotionCache, pageProps } = props
  return (
    <CacheProvider value={emotionCache}>
      <Head>
        <meta name="viewport" content="initial-scale=1, width=device-width" />
      </Head>
      <Themer>
        <CssBaseline />
        <Component {...pageProps} />
      </Themer>
    </CacheProvider>
  )
}

export default MyApp
