import {NativeStackScreenProps} from "@react-navigation/native-stack";
import {RootStackParamList} from "../../navigation/types.ts";
import {Alert, ScrollView, StyleSheet, View} from "react-native";
import {useState} from "react";
import {useWorkers} from "../../context/WorkersContext.tsx";
import {ActivityIndicator, Button, Card, TextInput, useTheme} from "react-native-paper";

type Props = NativeStackScreenProps<RootStackParamList, "Login">;

function LoginScreen({navigation} : Props) {
    const theme = useTheme();
    const { addWorker } = useWorkers();
    
    const [loading, setLoading] = useState(true);
    const [submitting, setSubmitting] = useState(false);
    
    const handleSubmit = async () => {
        

        try {
            setSubmitting(true);
            //todo

            Alert.alert("Sukces", "Zalogowano pomyślnie", [
                { text: "OK", onPress: () => navigation.goBack() },
            ]);
        } catch (err) {
            Alert.alert("Błąd", (err as Error).message);
        } finally {
            setSubmitting(false);
        }
    }

    if (loading) {
        return (
            <View style={[styles.centerContainer, { backgroundColor: theme.colors.background }]}>
                <ActivityIndicator size="large" color={theme.colors.primary} />
            </View>
        );
    }

    return (
        <ScrollView style={{ backgroundColor: theme.colors.background }} contentContainerStyle={styles.scrollContent}>
            <Card style={styles.card} mode="contained">
                <Card.Content style={styles.gap}>

                    //inputy do zmiany
                    <TextInput
                        label="Login"
                        mode="outlined"
                        value={firstName}
                        onChangeText={setFirstName}
                        editable={!submitting}
                        style={styles.input}
                        outlineColor={theme.colors.outline}
                        activeOutlineColor={theme.colors.primary}
                    />

                    <TextInput
                        label="Hasło"
                        mode="outlined"
                        value={lastName}
                        onChangeText={setLastName}
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
    centerContainer: {
        flex: 1,
        justifyContent: 'center',
        alignItems: 'center'
    },
    scrollContent: {
        padding: 16,
        marginTop: 25,
        flexGrow: 1
    },
    card: {
        borderRadius: 24,
        paddingVertical: 8
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
        marginTop: 16
    },
    flex1: {
        flex: 1
    }
});

export default LoginScreen;