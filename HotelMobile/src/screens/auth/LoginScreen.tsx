import {NativeStackScreenProps} from "@react-navigation/native-stack";
import {RootStackParamList} from "../../navigation/types.ts";
import {Alert, ScrollView, StyleSheet, View} from "react-native";
import {useState} from "react";
import {ActivityIndicator, Button, Card, Text, TextInput, useTheme} from "react-native-paper";
import {useAuth} from "../../context/AuthContext.tsx";
import {ApiError} from "../../types/errors.ts";
import {useFormErrors} from "../../hooks/useFormErrors.ts";

type Props = NativeStackScreenProps<RootStackParamList, "Login">;

function LoginScreen({navigation} : Props) {
    const theme = useTheme();
    
    const {login} = useAuth()
    const [username,setUsername] = useState("");
    const [password,setPassword] = useState("");
    const [loading, setLoading] = useState(false);
    const [submitting, setSubmitting] = useState(false);

    const {
        errors,
        generalError,
        clearFieldError,
        clearAllErrors,
        handleApiError
    } = useFormErrors();
    
    const handleSubmit = async () => {
        
        try {
            setSubmitting(true);
            await login({
                login: username.trim(),
                password: password.trim()
            })
        } catch (err) {
            handleApiError(err as ApiError);
            setSubmitting(false);
        }
    }
    
    return (
        <ScrollView style={{ backgroundColor: theme.colors.background }} contentContainerStyle={styles.scrollContent}>
            <Card style={styles.card} mode="contained">
                <Card.Content style={styles.gap}>
                    
                    <View style={styles.headerContainer}>
                        <Text variant="headlineMedium" style={[styles.title, { color: theme.colors.primary }]}>
                            Logowanie
                        </Text>
                    </View>
                    <TextInput
                        label="Login"
                        mode="outlined"
                        value={username}
                        onChangeText={(text) => {
                            setUsername(text)
                            clearFieldError("login")
                        }}
                        editable={!submitting}
                        style={styles.input}
                        outlineColor={theme.colors.outline}
                        activeOutlineColor={theme.colors.primary}
                    />
                    
                    <TextInput
                        label="Hasło"
                        mode="outlined"
                        secureTextEntry={true}
                        value={password}
                        onChangeText={(text) => {
                            setPassword(text)
                            clearFieldError("password")
                        }}
                        editable={!submitting}
                        style={styles.input}
                        outlineColor={theme.colors.outline}
                        activeOutlineColor={theme.colors.primary}
                    />

                    <View style={styles.buttons}>
                        <Button
                            mode="contained"
                            onPress={handleSubmit}
                            disabled={submitting}
                            loading={submitting}
                            style={styles.flex1}
                            buttonColor={theme.colors.primary}
                            textColor={theme.colors.onPrimary}
                        >
                            {submitting ? 'Logowanie...' : 'Zaloguj'}
                        </Button>
                    </View>
                </Card.Content>
            </Card>
        </ScrollView>
    );
}

const styles = StyleSheet.create({
    scrollContent: {
        padding: 16,
        flexGrow: 1,
        justifyContent: 'center', 
    },
    headerContainer: {
        alignItems: 'center',
        marginBottom: 8,
        gap: 4
    },
    title: {
        fontWeight: 'bold',
    },
    card: {
        borderRadius: 24,
        paddingVertical: 12
    },
    gap: {
        gap: 16
    },
    input: {
        backgroundColor: 'transparent'
    },
    buttons: {
        flexDirection: 'row',
        gap: 12,
        marginTop: 8
    },
    flex1: {
        flex: 1
    }
});

export default LoginScreen;